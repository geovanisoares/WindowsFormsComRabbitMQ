# [WindowsForms com RabbitMQ]

A solução consiste em exemplificar a comunicação assincrona usando RabbitMQ, utilizando dois serviços, um de envio dos dados de lead para lista e outro de consumo dessa lista e exibição em tabela dos últimos leads consumidos da lista.

**Resumo do funcionamento**

O serviço de envio (RabbitMQSender) envia os dados preenchidos em formulário para uma lista no broker do RabbitMQ.

O serviço de consumo (RabbiMQConsumer) consome os dados da lista do broker, gravando em um banco de dados em memória.

O serviço de consumo tem um timer configurado para ficar ouvindo o banco em memória e exibe os dados de leads em tela.

## Pré-requisitos ##

- IDE Visual Studio versão >= 2022.
- Docker versão >= 20.10.14.
- SDK .NET 6.
- Windows >= 10.

*A aplicação pode funcionar em versões anteriores das ferramentas acima, porém, é recomendado o uso das versões correspondentes informadas, pois correspondem aos utilizados no desenvolvimento e testes da aplicação.

## Preparando o ambiente ##

Com o docker em execução, abra o console do gerenciador de pacotes do visual studio ou o prompt de comando.

Suba um container do RabbitMQ, conforme linha abaixo:

	docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 -p 5672:5672 rabbitmq:3-management

Após executar o comando acima, o docker irá procurar localmente os arquivos necessário e, não encontrando, vai até o dockerhub e os baixa, este processo pode demorar um pouco dependendo da sua internet.   
Com o sucesso da instalação confira no docker desktop confira na aba "containers" se o RabbitMQ está em execução.

Caso o container já tenha sido criado, mas não esteja em execução, clique no botão de executar no docker desktop ou execute o seguinte comando no terminal:

	docker container start some-rabbit.

No Visual Studio, defina a solução para executar vários projetos de inicialização, caso ainda não esteja configurado.
	
	No gerenciado de soluções, clique com o botão direito na solução e depois em "Definir projetos de inicialização".
	Na tela que abrir, selecione "Varios projetos de inicialização", em seguida, na coluna "ação", coloque os dois projetos para "Iniciar", clique em "Aplicar" depois em "OK".

## Executando a aplicação ##

No Visual Studio, clique em "Iniciar", será aberto duas telas, uma correspondende a aplicação responsável por enviar os dados para lista de leads, um formulário com nome e idade, e outra responsável por consumir a lista de leads.

Você pode alimentar a lista de leads e assincronamente irá ver a aplicação consumidora exibir os dados do lead em tela, com o id gerado para o lead e data e hora de inserção desse registro.

Caso queira comprovar a assincronia não bloqueante, execute apenas o servido de envio (RabbitMQSender), registre quandos leads quiser, depois abra a aplicação consumidora (RabbitMQConsumer), você verá a aplicação iniciando, consumindo a fila e exibindo em tela os dados que estavam aguardando para serem consumidos.

Esta solução é exemplificativa, após o consumo da fila pelo consumer, atualmente o dado é salvo em um 
banco de dados em memória e permanece na fila por toda vida útil do container, em aplicações reais, 
após o dado ser consumido, normalmente o mesmo é retirado da fila depois de processado e, se for o caso, 
persistido em banco de dados pela aplicação consumidora.

Caso queira ver o funcionamento do RabbitMQ a partir de uma interface gráfica, com o container em execução, abra seu navegador e vá até localhost:8080.   
  Será aberto a tela de login do RabbitMQ, o usuário é "guest" e a senha é "guest" também. Clique em "Login".
  Na aba "Queues" será mostrada todas as filas criadas, se você registrou pelo menos um lead no serviço de envio (RabbitMQSender), aparecerá a fila "LeadQueue".
  Clicando nela abrirá uma página contendo gráficos, informações sobre a fila e o histórico das mensagens enviadas.
  Você pode manter esta tela aberta e registrar leads, assim poderá ver o RabbitMQ processando a mensagem e mostrando as alterações no gráfico.


## Problemas conhecidos ##

 - O Docker, na versão citada acima, roda no WSL2 (Windows Subsystem for Linux) do Windows, caso você não tenha ou tenha uma versão incompatível com o Docker, o mesmo irá solicitar o update e informará a referência para instalação / update.   
 Para saber mais sobre o WLS: https://learn.microsoft.com/pt-br/windows/wsl/about.