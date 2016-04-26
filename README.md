Readme Spryd v0.1

--- INSTALLATION ---

Aller sur Dreamspark Efrei http://e5.onthehub.com/d.ashx?s=whgiur6utj
Se connecter et télécharger "Visual Studio 2015 Entreprise""

Installer MySQL for Visual Studio https://dev.mysql.com/downloads/windows/visualstudio/

Dans le dossier où vous voulez que le projet soit (le sous-dossier spryd-serveur se créé automatiquement) :
git clone https://gitlab.com/spryd-team/spryd-serveur.git
Se connecter avec ses identifiants Gitlab pour lancer le clone

Lancer la solution avec VS en double cliquant sur Spryd.Server.sln

Démarrer la Base de données avec
- Soit Xampp : cliquer sur Start pour Apache, cliquer sur Start pour MySQL
- Soit MySQL Wokbench : dans le menu Navigator -> Onglet Instance -> Startup / Shutdown, cliquer sur Start Server
- Soit ne rien faire si vous souhaitez utiliser la base de données distante (TODO: vérifier que l'on a bien accès à celle ci en dehors d'un serveur OVH)

Importer le jeu de données si cela n'est pas fait

Dans Spryd.Serveur -> Models -> Dal.cs, vérifier que le connectionString soit bon pour vous connecter à la BDD

--- LANCEMENT ---

Cliquer sur le bouton vert de lancement pour Compiler et lancer le serveur de services REST

Accéder à l'url /User/{id} pour tester le bon fonctionnement, avec {id} un identifiant de user existant dans la base