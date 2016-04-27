Readme Spryd v0.1

--- INSTALLATION ---

Aller sur Dreamspark Efrei http://e5.onthehub.com/d.ashx?s=whgiur6utj
Se connecter et t�l�charger "Visual Studio 2015 Entreprise""

Dans le dossier o� vous voulez que le projet soit (le sous-dossier spryd-serveur se cr�� automatiquement) :
git clone https://gitlab.com/spryd-team/spryd-serveur.git
Se connecter avec ses identifiants Gitlab pour lancer le clone

Lancer la solution avec VS en double cliquant sur Spryd.Server.sln

D�marrer la Base de donn�es avec
- Soit Xampp : cliquer sur Start pour Apache, cliquer sur Start pour MySQL
- Soit MySQL Wokbench : dans le menu Navigator -> Onglet Instance -> Startup / Shutdown, cliquer sur Start Server
- Soit ne rien faire si vous souhaitez utiliser la base de donn�es distante (TODO: v�rifier que l'on a bien acc�s � celle ci en dehors d'un serveur OVH)

Importer le script de cr�ation de base si ce n'est pas fait.

Aller sur la base cr��e (sprydioflxadm) et importer le jeu de donn�es si cela n'est pas fait

Dans Spryd.Serveur -> Models -> Dal.cs, v�rifier que le connectionString soit bon pour vous connecter � la BDD

--- LANCEMENT ---

Cliquer sur le bouton vert de lancement pour Compiler et lancer le serveur de services REST

Acc�der � l'url /User/{id} pour tester le bon fonctionnement, avec {id} un identifiant de user existant dans la base

La page /Help permet de visualiser la documentation.