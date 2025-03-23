#!/bin/bash

# Configuration
APP_NAME="yes_location"
DEPLOY_PATH="/home/data/$APP_NAME"
BACKUP_PATH="/home/data/backups/$APP_NAME"
TIMESTAMP=$(date +%Y%m%d_%H%M%S)

# Création des dossiers nécessaires
mkdir -p $DEPLOY_PATH
mkdir -p $BACKUP_PATH

# Sauvegarde de la base de données actuelle si elle existe
if [ -d "$DEPLOY_PATH/mysql_data" ]; then
    echo "Sauvegarde de la base de données..."
    docker-compose -f $DEPLOY_PATH/docker-compose.yml -f $DEPLOY_PATH/docker-compose.prod.yml exec -T mysql mysqldump -u root -p${MYSQL_ROOT_PASSWORD} ${MYSQL_DATABASE} > $BACKUP_PATH/db_backup_$TIMESTAMP.sql
fi

# Sauvegarde des fichiers de configuration
if [ -f "$DEPLOY_PATH/.env" ]; then
    cp $DEPLOY_PATH/.env $BACKUP_PATH/.env_$TIMESTAMP
fi

# Arrêt des conteneurs existants
if [ -f "$DEPLOY_PATH/docker-compose.yml" ]; then
    echo "Arrêt des conteneurs existants..."
    docker-compose -f $DEPLOY_PATH/docker-compose.yml -f $DEPLOY_PATH/docker-compose.prod.yml down
fi

# Copie des nouveaux fichiers
echo "Copie des nouveaux fichiers..."
cp -r ./* $DEPLOY_PATH/

# Restauration des fichiers de configuration sauvegardés
if [ -f "$BACKUP_PATH/.env_$TIMESTAMP" ]; then
    cp $BACKUP_PATH/.env_$TIMESTAMP $DEPLOY_PATH/.env
fi

# Démarrage des conteneurs
echo "Démarrage des conteneurs..."
cd $DEPLOY_PATH
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d --build

# Attente que MySQL soit prêt
echo "Attente que MySQL soit prêt..."
sleep 30

# Exécution des migrations
echo "Exécution des migrations..."
docker-compose -f docker-compose.yml -f docker-compose.prod.yml exec -T backend dotnet ef database update --project YesLocation.Api

# Nettoyage des anciennes sauvegardes (garder les 5 dernières)
echo "Nettoyage des anciennes sauvegardes..."
cd $BACKUP_PATH
ls -t | tail -n +6 | xargs -r rm --

echo "Déploiement terminé avec succès!" 