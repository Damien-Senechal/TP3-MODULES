# Sprint 2 — Reponses

## Exercice 1 — Cartographie

### 1.1 Classes et interfaces publiques

IReservationRepository
IRoomRepository
IConfirmationSender
ICleaningNotifier
ICleaningPolicy
IPricingStrategy

BookingService
BillingService
HousekeepingScheduler

EmailSender
SmsSender
InMemoryReservationStore
InMemoryRoomStore
InvoiceGenerator
TaxCalculator
PricingStrategyFactory
RoomAssigner
StandardCleaningPolicy
VipCleaningPolicy
FlexibleCancellationPolicy
ModerateCancellationPolicy
StrictCancellationPolicy
NonRefundableCancellationPolicy
StandardPricingStrategy
SuitePricingStrategy
FamilyPricingStrategy

Reservation
Room
RoomType
Invoice
InvoiceLine
CleaningTask


### 1.2 Graphe de dependances

(Decrivez ou collez un schema)

![Graphe de dépendances](schema.png)

### 1.3 Clusters identifies

- Cluster 1 : ...
  - Justification : ...
- Cluster 2 : ...
  - Justification : ...
- Cluster 3 : ...
  - Justification : ...

-Cluster Booking : BookingService, RoomAssigner, IConfirmationSender, IReservationRepository, IRoomRepository, Reservation, Room, RoomType
  -Ces classes changent quand les règles de réservation, check-in ou attribution de chambre évoluent. L'acteur est le réceptionniste.
-Cluster Billing : BillingService, InvoiceGenerator, TaxCalculator, PricingStrategyFactory, IPricingStrategy, Invoice, InvoiceLine
  -Ces classes changent quand la TVA, les tarifs ou le format de facture changent. L'acteur est le comptable.
-Cluster Housekeeping : HousekeepingScheduler, ICleaningPolicy, ICleaningNotifier, CleaningTask, StandardCleaningPolicy, VipCleaningPolicy
  -Ces classes changent quand la politique de ménage ou les canaux de notification changent. L'acteur est la gouvernante.

---

## Exercice 2 — Decoupage

### Modules crees

| Module | Justification |
|-------|---------------|
| ... | ... |

| Module               | Justification                                                                                             |
|----------------------|-----------------------------------------------------------------------------------------------------------|
| Hotel.Booking        | Regroupe les règles de réservation, check-in/check-out et attribution de chambre. Metier : réceptionniste |
| Hotel.Billing        | Regroupe la facturation, le calcul de TVA et les stratégies de tarification. Metier : comptable           |
| Hotel.Housekeeping   | Regroupe le planning de ménage et les politiques de nettoyage. Metier : gouvernante                       |
| Hotel.Infrastructure | Contient les implémentations concrètes (stores en mémoire, EmailSender, SmsSender). Metier : none         |
| Hotel.Runner         | Assemble les modules via DI, projet qui reference tous le monde                                           |

### Justification par principe

- **CCP** : (expliquez pourquoi vous avez regroupe certaines classes)
- **CRP** : (expliquez pourquoi vous avez separe certaines classes)
- **REP** : (expliquez la coherence de chaque module)

CCP : les classes qui changent ensemble pour la même raison sont dans le même module. BillingService, InvoiceGenerator et TaxCalculator changent tous si la TVA change, ils sont dans Hotel.Billing. HousekeepingScheduler et StandardCleaningPolicy changent si la politique de ménage change, ils sont dans Hotel.Housekeeping. On n'a pas mélangé des classes qui répondent à des acteurs différents.

CRP : on a évité de forcer des dépendances inutiles. Chaque module a sa propre vision des données via des DTOs dédiés (BillingReservationDto, HousekeepingReservationDto). Billing ne connaît pas le GuestPhone, Housekeeping ne connaît pas le TotalPrice. EmailSender et SmsSender sont dans Infrastructure et non dans les modules métier, donc ajouter un canal push ne touche pas Booking ou Housekeeping.

REP : chaque module métier est cohérent et pourrait être réutilisé indépendamment. Hotel.Billing peut être embarqué dans n'importe quel système de facturation hôtelière sans emporter le ménage. Hotel.Housekeeping peut tourner seul dans un outil de gestion du personnel. Hotel.Infrastructure en revanche n'est pas réutilisable seule, c'est normal : c'est le câblage concret, pas une librairie.

---

## Exercice 3 — Test de la modification

### Scenario A — Politique de menage

- Fichiers modifies : Hotel.Housekeeping/CleaningPolicy.cs (1 fichier)
- Modules impactes : Hotel.Housekeeping (1 module)
On change AddDays(3) en AddDays(2) dans StandardCleaningPolicy. Aucun autre module n'est au courant de ce changement.

### Scenario B — Taux de TVA

- Fichiers modifies : Hotel.Billing/TaxCalculator.cs (1 fichier)
- Modules impactes : Modules impactés : Hotel.Billing (1 module)
On change la constante AccommodationTvaRate de 0.10m à 0.12m. Booking et Housekeeping ne connaissent pas TaxCalculator, ils ne sont pas touchés.

### Scenario C — Push notification

- Fichiers crees : 1 créé
- Fichiers modifies : 1 modifié
- Modules metier impactes : 0

On crée PushNotificationSender dans Hotel.Infrastructure qui implémente IConfirmationSender. On modifie le câblage dans ServiceRegistration ou Program.cs pour l'enregistrer.

Hotel.Booking ne connaît que IConfirmationSender, peu importe si derrière c'est un email, un SMS ou un push. L'interface appartient au module Booking, l'implémentation est dans Infrastructure.

### Comparaison avec le code de depart

Le principe qui garantit que le scénario A ne touche qu'un seul module est le CCP (Common Closure Principle) : la politique de nettoyage et son implémentation sont dans le même module Hotel.Housekeeping. Tout ce qui change ensemble vit ensemble.
Le principe qui garantit que le scénario C n'impacte pas les modules métier est le DIP via les ports & adapters : Hotel.Booking définit IConfirmationSender (le port), PushNotificationSender est un adapter dans Infrastructure. Le module métier ne dépend pas de l'implémentation concrète.
