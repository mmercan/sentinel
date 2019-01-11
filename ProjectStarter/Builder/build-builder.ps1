#docker build -t aspcore-full-framework:2.0.5 -f ./docker/Builder/dockerfile .
#docker build --no-cache -t mmercan/aspcore-full-framework:4.6.2 -f ./dockerfile .

docker build -t mmercan/aspcore-builder-framework:4.6.2 -f ./dockerfile .