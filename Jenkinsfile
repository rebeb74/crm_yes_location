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
    stage('Build Frontend') {
      steps {
        sh """
            cd frontend
            . ${NVM_DIR}/nvm.sh
            nvm use --silent 16.20.0
            npm install
            npm run build
            # Copier les fichiers de build vers le dossier Apache
            sudo cp -r dist/* /home/data/yes-location/
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
