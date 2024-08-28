# Define variables for the image name and container name
IMAGE_NAME = currencyconverter
CONTAINER_NAME = currencyconverter_container

# Target to build the Docker image
build:
	docker build -t $(IMAGE_NAME) .

# Target to run the Docker container
up: build
	docker run -d --name $(CONTAINER_NAME) -p 80:80 $(IMAGE_NAME)

# Target to stop and remove the Docker container
down:
	docker stop $(CONTAINER_NAME) || true
	docker rm $(CONTAINER_NAME) || true

# Target to build and run the container in one step
restart: down up

# Target to remove the Docker image
clean:
	docker rmi $(IMAGE_NAME) || true
