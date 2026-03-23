# Projet_fil-rouge_interact_3D

Avec Unity, réalisation d’une expérience de réalité virtuelle, fonctionnelle, immersive et cohérente pour mettre en pratique l’apprentissage.

## 🛠️ Outils et Technologies
* **Moteur 3D :** Unity
* **Framework VR :** XR Interaction Toolkit (XRI) pour la gestion avancée de la réalité virtuelle, des déplacements et des interactions physiques.
* **Cible Matérielle :** Optimisé pour les casques Meta Quest.

---

## ✨ Fonctionnalités et Avancées du Projet

### 🌍 Environnement & Ambiance
* **Création de la scène :** Mise en place d'un environnement 3D complet comprenant un espace intérieur détaillé (une chambre) et un décor extérieur (montagnes).
* **Éclairage :** Les lumières ont été placées uniquement à l'intérieur de la chambre (aucun éclairage en extérieur). Ajout d'une lampe de chevet interactive dont la lumière peut être directement saisie (Grab) et déplacée physiquement par le joueur dans la pièce.

### 🎮 Mécaniques de Réalité Virtuelle
* **Locomotion VR :** Intégration d'un système de déplacement complet grâce à XRI (mouvements continus au stick, système de téléportation, gestion de la rotation fluide ou par à-coups).
* **Interactions Physiques :** Possibilité pour le joueur de saisir (Grab), manipuler et interagir physiquement en 3D avec les différents objets présents dans la scène.

### 📺 Multimédia & Son
* **Télévision Interactive :** Ajout d'une télévision fonctionnelle qui diffuse un clip vidéo directement dans l'environnement virtuel.
* **Audio Spatialisé :** Configuration du son de la télévision en 3D spatialisée (le volume et la provenance du son s'adaptent naturellement à la position et aux mouvements de la tête du joueur).

### 🖥️ Interface Utilisateur (UI) & Saisie VR
* **Panneau de Contrôle :** Création d'une interface utilisateur interactive permettant de :
  * Gérer dynamiquement le volume de la télévision.
  * Afficher les informations de la météo en direct.
  * Modifier les paramètres de base de l'environnement (changement des coordonnées de latitude et longitude).
* **Clavier Natif Meta Quest :** Surcharge et configuration spécifique du fichier `AndroidManifest.xml` (`<uses-feature android:name="oculus.software.overlay_keyboard" ... />`) pour forcer l'utilisation du clavier virtuel système de Meta. Cela garantit une saisie de texte beaucoup plus ergonomique et native lors des interactions avec l'UI en réalité virtuelle.