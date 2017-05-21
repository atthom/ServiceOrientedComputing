								### ServiceOrientedComputing ###

Pour ce projet le client intranet et le server ont été dévellopé en réutilisant ce que nous avions vu dans
les TD correspondant. 

Nous avons ensuite essayé de mettre en place une connection sécurisé pour le client extranet. 
Pour ce faire, nous avons crées un certificat temporaire du côté server puis à partir de ce certificat 
nous avons crée un certificat pour le client. Nous avons bien évidemment modifié les fichiers app.config 
du client et du server en conséquence. Sachant que le binding entre les deux est de type wsHttpBinding avec
une sécurité au niveau du transport qui est de type credential. Tout cela a été réalisé en suivant les deux 
tutoriels suivant principalement. 
  * https://msdn.microsoft.com/en-us/library/ff648498.aspx
  * https://msdn.microsoft.com/en-us/library/ff648360.aspx

Malheureusement nous sommes confrontés à l'erreur suivante: "Le certificat distant n'est pas valide selon 
la procédure de validation." et nous n'avons pas trouvé de solution pour cette dernière.