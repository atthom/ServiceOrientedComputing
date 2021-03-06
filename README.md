# ServiceOrientedComputing

## Solution

Notre client se connecte à notre serveur et lui envoie des adresses données par l'utilisateur.

Nous avons développé deux clients un client pour l'intranet et un autre client pour l'extranet.

Nous avons essayé de mettre en place une connection sécurisé pour le client extranet. Pour ce faire,
nous avons crées un certificat temporaire du côté server (cf. dossier certificate) puis à partir de ce
certificat nous avons crée un certificat pour le client (cf. dossier Client_Certificate). Nous avons bien
évidemment modifié les fichiers app.config du client et du server en conséquence. Sachant que le binding entre
les deux est de type wsHttpBinding avec une sécurité au niveau du transport qui est de type credential. Tout 
cela a été réalisé en suivant les deux tutoriels suivant (principalement):
  * https://msdn.microsoft.com/en-us/library/ff648498.aspx
  * https://msdn.microsoft.com/en-us/library/ff648360.aspx

Malheureusement nous sommes confrontés à l'erreur suivante: "Le certificat distant n'est pas valide selon 
la procédure de validation." et nous n'avons pas trouvé de solution pour cette dernière. Nous avons donc 
commenté la partie qui fait référence au certificat dans le fichier app.config du server. A noter que dans
le cas contraire il aurait fallut suivre l'étape 2 du tuto 1 pour ajouter le certificat sur le port 8083 
de votre machine (pas très pratique mais en théorie un serveur n'est pas voué à être changé de machine souvent).  

## Phase de déploiement:

- Executer le projet serveur avec Visual Studio en mode administrateur.
- Executer le projet client avec Visual Studio.
- Suivre les log sur l'invite de commande.

Note: Pour aller plus vite le client intranet vous propose une demo d'entrée, ce qui vous permet de voir directement le 
résultat d'une utilisation de notre service (en fait deux utilisation: une avec des adresses précises, une autre avec 
des noms de villes simplement).