/* groovylint-disable-next-line CompileStatic */
pipeline {
  agent any
  environment {
        NVM_DIR = '/var/lib/jenkins/.nvm'
        GIT_SSH_COMMAND = "ssh -vvv -i ${env.JENKINS_HOME}/.ssh/id_ed25519"
  }
  stages {
    stage('Pull') {
      steps {
        sh '''
            git checkout main
            git reset --hard HEAD
            git pull
        '''
      }
    }
    stage('Setup Node.js') {
      steps {
        sh """
            # Installer NVM si pas déjà installé
            if [ ! -d "${NVM_DIR}" ]; then
              curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash
            fi
            # Charger NVM
            . ${NVM_DIR}/nvm.sh
            # Installer Node.js 18 si pas déjà installé
            nvm install 18.19.0
            nvm use 18.19.0
        """
      }
    }
    stage('Build Frontend') {
      steps {
        sh """
            cd frontend
            . ${NVM_DIR}/nvm.sh
            nvm use 18.19.0
            npm install
            npm run build
            # Créer le dossier de destination s'il n'existe pas
            mkdir -p /home/data/yes-location
            # Copier tous les fichiers de build
            cp -r dist/* /home/data/yes-location/
            # S'assurer que les permissions sont correctes
            chown -R www-data:www-data /home/data/yes-location
        """
      }
    }
    stage('Build and Deploy Backend') {
      steps {
        sh '''
            cd backend
            # Build l'image Docker
            docker build -t yes-location-api .
            # Arrêter et supprimer l'ancien container s'il existe
            docker stop yes-location-api || true
            docker rm yes-location-api || true
            # Démarrer le nouveau container
            docker run -d --name yes-location-api \
              -p 5000:80 \
              -v /home/data/yes-location-api:/app/data \
              yes-location-api
        '''
      }
    }
  }
  post {
    always {
      cleanWs()
    }
  }
}
