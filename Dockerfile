# Para rodar iniciar essa mensagem basta rodar o seguinte comando
# docker run -d --name my-rabbit -p 5672:5672 -p 15672:15672 --restart=always --hostname rabbitmq-master $(nome da imagem gerada com dockerfile)

FROM rabbitmq:3-management

VOLUME ["C:/docker/rabbitmq", "/var/lib/rabbitmq"]

RUN rabbitmq-plugins enable rabbitmq_management
RUN rabbitmq-plugins enable rabbitmq_stomp
RUN rabbitmq-plugins enable rabbitmq_web_stomp

EXPOSE 5672 15672
EXPOSE 5674 15674