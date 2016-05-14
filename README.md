Readme Spryd v0.1

--- INSTALLATION ---

Aller sur Dreamspark Efrei http://e5.onthehub.com/d.ashx?s=whgiur6utj
Se connecter et télécharger "Visual Studio 2015 Entreprise""

Dans le dossier où vous voulez que le projet soit (le sous-dossier spryd-serveur se créé automatiquement) :
git clone https://gitlab.com/spryd-team/spryd-serveur.git
Se connecter avec ses identifiants Gitlab pour lancer le clone

Lancer la solution avec VS en double cliquant sur Spryd.Server.sln

Démarrer la Base de données avec
- Soit Xampp : cliquer sur Start pour Apache, cliquer sur Start pour MySQL
- Soit MySQL Wokbench : dans le menu Navigator -> Onglet Instance -> Startup / Shutdown, cliquer sur Start Server
- Soit ne rien faire si vous souhaitez utiliser la base de données distante (TODO: vérifier que l'on a bien accès à celle ci en dehors d'un serveur OVH)

Importer le script de création de base si ce n'est pas fait.

Aller sur la base créée (sprydioflxadm) et importer le jeu de données si cela n'est pas fait

Dans Spryd.Serveur -> Models -> Dal.cs, vérifier que le connectionString soit bon pour vous connecter à la BDD

--- Configuration IIS ---
- Aller dans panneau de configuration
- Desinstaller un programme
- Activer ou désactiver les fonctionnalités Windows
- Cocher toute l'arborescence IIS (TOUUUTE !)

- Ouvrir Gestionnaire de services IIS
- Créer un site DEMANDER A HAROON LA SUITE
- Vérifier que la version .NET utilisée est V4 pour le Site
- Si non, passer en V4 via les paramètres.
  (si il y a une erreur, c'est que V4 n'est pas bien installé, il faut lancer la commande <todo: retrouver>)
  
- Il faut également vérifier que dans Panneau de config windows > Parefeu > Paramètres avancés > Connexions entrantes > 
  vérif que le port associé au serveur est bien autorisé en TCP pour toutes les machines
  
Attention on ne peut pas déployer l'application sur IIS en mode debug. Sur IIS Express (intégré à VS) on peut.

--- LANCEMENT ---

Cliquer sur le bouton vert de lancement pour Compiler et lancer le serveur de services REST

Accéder à l'url /User/{id} pour tester le bon fonctionnement, avec {id} un identifiant de user existant dans la base

La page /Help permet de visualiser la documentation.