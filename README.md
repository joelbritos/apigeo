# APIGEO
El proyecto se encuentra alojado en https://drive.google.com/drive/folders/1ne4GI8pOlOLgteV4IOqTckexrRFzgt3b?usp=sharing

Debe descargar ambos archivos zipeados, ya que forman parte de una solución.


# Requisitos
1. Tener instalado Docker-Desktop
2. Tener instalado Visual Studio Code o Visual Studio

# Pasos para la ejecución
1. Ingresar al directorio principal de ambos proyectos, abrir la consola (terminal/powershell) y ejectura "docker build -t "username"/"nombreDelProyecto"
2. El nombre de las imagenes de Docker debería quedar algo asi: "userName"/apigeo y "userName"/geocodificador
3. Ingresar al directorio raíz de APIGEO(allí se encuentra el Docker-compose.yml) y ejecutar el comando "docker-compose up"
4. Debido a un cambio de versión en el docker-compose.yml, en la primera ejecución fallá el contenedor de APIGEO, pero por ahora se deja detenido
5. Se debe detener también el contenedor de GEOCODIFICADOR. 
6. Desde el dashboard de Docker, acceder al CLI del contenedor Kafka y ejecutar los siguientes comandos:
    kafka-topics --create --bootstrap-server kafka:9092 --replication-factor 1 --partitions 1 --topic procesar /
    kafka-topics --create --bootstrap-server kafka:9092 --replication-factor 1 --partitions 1 --topic enProceso /
    kafka-topics --create --bootstrap-server kafka:9092 --replication-factor 1 --partitions 1 --topic Procesados /
7. Una vez creado los topics, iniciar el contenedor APIGEO y GEOCODIFICADOR

Este paso se debe a que la ejecución de esos comandos directamente desde el .yml provocan que se apague el contenedor de Kafka, y al no crearse automáticamente, los contenedores de APIGEO y GEOCODIFICADOR no pueden subscribirse a topics inexistentes, y arrojan excepción. De todos modos se puede revisar con más tiempo.
