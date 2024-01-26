Amine et Noella

--- On a réussi à:
-enlever le texte inutile dans l'écran GameoOver 
-mettre le high Score et à l'afficher à l'ecran Gameover et Win (avec le score actuel du joueur)
-faire de save/load (dan sle fichier bestScore.txt):
-régler les erreurs d'indentation 
-nommer correctement les const 
-nommer correctement les var publiques


--- REGLE DU JEU:
Le joueur a 3 vies. Il perd une vie lors d'une collision avec un monstre. Lorsque toutes les vies sont épuisées, c'est un GameOver. OU lorsque le temps de jeu (defini à 60 secondes par défaut) est écoulé, c'est aussi un GameOver. 
Si le joueur atteint la sortie, il a gagné. 
Le score augmente en fonction du temps (de 3 points toutes les 20s). Le score est affiché à l'écran Gameover si échec ou à l'écran "Gagné" si le joueur a atteint la sortie.
 

--- Sauvegarde/Récupération du meilleur score à partir dun fichier etxte:
On a méthode 'WriteBestScore()' pour écrire le meilleur score dans le fichier 'bestScore.txt'. Puis la méthode 'ReadBestScore' pour lire le meilleur score depuis le fichier texte


