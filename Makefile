IMAGE_NAME := aex/movies-test
VERSION := 0.1.0

build-image:
	sudo docker build -f ./AEXMovies/Dockerfile AEXMovies -t $(IMAGE_NAME):latest -t $(IMAGE_NAME):$(VERSION)