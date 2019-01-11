#docker build -t aspcore-full-framework:2.0.5 -f ./docker/Builder/dockerfile .
#docker build --no-cache -t mmercan/aspcore-full-framework:4.6.2 -f ./dockerfile .

docker build -t mmercan/vs2017 -f ./vs2017/dockerfile -m 4GB .