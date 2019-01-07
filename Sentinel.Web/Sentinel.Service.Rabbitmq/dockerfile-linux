FROM rabbitmq:3-management

# ENV ABBITMQ_ERLANG_COOKIE = SWQOKODSQALRPCLNMEQG
# ENV RABBITMQ_DEFAULT_USER = rabbitmq
# ENV RABBITMQ_DEFAULT_PASS = rabbitmq
# ENV RABBITMQ_DEFAULT_VHOST = /

COPY ./config/rabbit_ac2b7e114d77_2019-1-7.json /etc/rabbitmq/definitions.json
COPY ./config/enabled_plugins /etc/rabbitmq/enabled_plugins
COPY ./config/rabbitmq.conf /etc/rabbitmq/rabbitmq.conf


#RUN rabbitmqctl wait /var/run/rabbitmq/pid
# ENTRYPOINT [ "executable" ]

#RUN ln -s usr/local/bin/docker-entrypoint.sh
# ENTRYPOINT ["docker-entrypoint.sh"]