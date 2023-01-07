# [WindowsForms com RabbitMQ]

A solu��o consiste em exemplificar a comunica��o assincrona usando RabbitMQ, utilizando dois servi�os, um de envio dos dados de lead para lista e outro de consumo dessa lista e exibi��o em tabela dos �ltimos leads consumidos da lista.

**Resumo do funcionamento**

O servi�o de envio (RabbitMQSender) envia os dados preenchidos em formul�rio para uma lista no broker do RabbitMQ.

O servi�o de consumo (RabbiMQConsumer) consome os dados da lista do broker, gravando em um banco de dados em mem�ria.

O servi�o de consumo tem um timer configurado para ficar ouvindo o banco em mem�ria e exibe os dados de leads em tela.

## Pr�-requisitos ##

- IDE Visual Studio vers�o >= 2022.
- Docker vers�o >= 20.10.14.
- SDK .NET 6.
- Windows >= 10.

*A aplica��o pode funcionar em vers�es anteriores das ferramentas acima, por�m, � recomendado o uso das vers�es correspondentes informadas, pois correspondem aos utilizados no desenvolvimento e testes da aplica��o.

## Preparando o ambiente ##

Com o docker em execu��o, abra o console do gerenciador de pacotes do visual studio ou o prompt de comando.

Suba um container do RabbitMQ, conforme linha abaixo:

	docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 -p 5672:5672 rabbitmq:3-management

Ap�s executar o comando acima, o docker ir� procurar localmente os arquivos necess�rio e, n�o encontrando, vai at� o dockerhub e os baixa, este processo pode demorar um pouco dependendo da sua internet.   
Com o sucesso da instala��o confira no docker desktop confira na aba "containers" se o RabbitMQ est� em execu��o.

Caso o container j� tenha sido criado, mas n�o esteja em execu��o, clique no bot�o de executar no docker desktop ou execute o seguinte comando no terminal:

	docker container start some-rabbit.

No Visual Studio, defina a solu��o para executar v�rios projetos de inicializa��o, caso ainda n�o esteja configurado.
	
	No gerenciado de solu��es, clique com o bot�o direito na solu��o e depois em "Definir projetos de inicializa��o".
	Na tela que abrir, selecione "Varios projetos de inicializa��o", em seguida, na coluna "a��o", coloque os dois projetos para "Iniciar", clique em "Aplicar" depois em "OK".

## Executando a aplica��o ##

No Visual Studio, clique em "Iniciar", ser� aberto duas telas, uma correspondende a aplica��o respons�vel por enviar os dados para lista de leads, um formul�rio com nome e idade, e outra respons�vel por consumir a lista de leads.

Voc� pode alimentar a lista de leads e assincronamente ir� ver a aplica��o consumidora exibir os dados do lead em tela, com o id gerado para o lead e data e hora de inser��o desse registro.

Caso queira comprovar a assincronia n�o bloqueante, execute apenas o servido de envio (RabbitMQSender), registre quandos leads quiser, depois abra a aplica��o consumidora (RabbitMQConsumer), voc� ver� a aplica��o iniciando, consumindo a fila e exibindo em tela os dados que estavam aguardando para serem consumidos.

Esta solu��o � exemplificativa, ap�s o consumo da fila pelo consumer, atualmente o dado � salvo em um 
banco de dados em mem�ria e permanece na fila por toda vida �til do container, em aplica��es reais, 
ap�s o dado ser consumido, normalmente o mesmo � retirado da fila depois de processado e, se for o caso, 
persistido em banco de dados pela aplica��o consumidora.

Caso queira ver o funcionamento do RabbitMQ a partir de uma interface gr�fica, com o container em execu��o, abra seu navegador e v� at� localhost:8080.   
  Ser� aberto a tela de login do RabbitMQ, o usu�rio � "guest" e a senha � "guest" tamb�m. Clique em "Login".
  Na aba "Queues" ser� mostrada todas as filas criadas, se voc� registrou pelo menos um lead no servi�o de envio (RabbitMQSender), aparecer� a fila "LeadQueue".
  Clicando nela abrir� uma p�gina contendo gr�ficos, informa��es sobre a fila e o hist�rico das mensagens enviadas.
  Voc� pode manter esta tela aberta e registrar leads, assim poder� ver o RabbitMQ processando a mensagem e mostrando as altera��es no gr�fico.


## Problemas conhecidos ##

 - O Docker, na vers�o citada acima, roda no WSL2 (Windows Subsystem for Linux) do Windows, caso voc� n�o tenha ou tenha uma vers�o incompat�vel com o Docker, o mesmo ir� solicitar o update e informar� a refer�ncia para instala��o / update.   
 Para saber mais sobre o WLS: https://learn.microsoft.com/pt-br/windows/wsl/about.