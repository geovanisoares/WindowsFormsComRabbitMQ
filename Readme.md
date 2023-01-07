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

*A aplica��o pode funcionar em vers�es anteriores das ferramentas acima, por�m, � recomendado o uso das vers�es correspondentes informadas, pois correspondem aos utilizados no desenvolvimento e testes da aplica��o.

## Preparando o ambiente ##

Com o docker em execu��o, abra o console do gerenciador de pacotes do visual studio ou o prompt de comando.

Suba um container do RabbitMQ, conforme linha abaixo:

	docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 rabbitmq:3-management

Caso o container j� tenha sido criado, execute o seguinte comando:

	docker container start some-rabbit.

Defina a solu��o para executar v�rios projetos de inicializa��o, caso ainda n�o esteja configurado.
	
	No gerenciado de solu��es, clique com o bot�o direito e depois em "Definir projetos de inicializa��o".
	Na tela que abrir, selecione "Varios projetos de inicializa��o", em seguida, na coluna "a��o", coloque os dois projetos para "Iniciar", clique em "Aplicar" depois em "OK".

## Executando a aplica��o ##

Clique em iniciar no visual studio, ser� aberto duas telas, uma correspondende a aplica��o respons�vel por enviar os dados para lista de leads, e outra respons�vel por consumir a lista de leads.

Voc� pode alimentar a lista de leads e assincronamente ir� ver a aplica��o consumidora exibir os dados do lead em tela, com o id gerado para o lead e data e hora de inser��o desse registro.

Caso queira comprovar a assincronia n�o bloqueante, execute apenas o servido de envio (RabbitMQSender), registre quandos leads quiser, depois abra a aplica��o consumidora (RabbitMQConsumer), voc� ver� a aplica��o iniciando e consumindo na fila e exibindo em tela os dados que estavam aguardando para serem consumidos.

Esta solu��o e exemplificativa, ap�s o consumo da fila pelo consumer, atualmente o dado � salvo em um 
banco de dados em mem�ria e permanece na fila por toda vida �til do container, em aplica��es reais, 
ap�s o dado ser consumido, normalmente o mesmo � retirado da fila depois de processado e, se for o caso, 
persistido em banco de dados pela aplica��o consumidora.