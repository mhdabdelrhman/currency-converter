# Define variables for the image name and container name
IMAGE_NAME = currencyconverter
CONTAINER_NAME = currencyconverter_container

# Target to build the Docker image
build:
	docker build -t $(IMAGE_NAME) .

# Target to run the Docker container
start: build
	docker run -d --name $(CONTAINER_NAME) -p 8080:8080 $(IMAGE_NAME)

# Target to stop and remove the Docker container
stop:
	docker stop $(CONTAINER_NAME) || true
	docker rm $(CONTAINER_NAME) || true

# Target to build and run the container in one step
restart: stop start

# Target to remove the Docker image
clean:
	docker rmi $(IMAGE_NAME) || true
