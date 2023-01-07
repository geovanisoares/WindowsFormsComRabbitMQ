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

*A aplicação pode funcionar em versões anteriores das ferramentas acima, porém, é recomendado o uso das versões correspondentes informadas, pois correspondem aos utilizados no desenvolvimento e testes da aplicação.

## Preparando o ambiente ##

Com o docker em execução, abra o console do gerenciador de pacotes do visual studio ou o prompt de comando.

Suba um container do RabbitMQ, conforme linha abaixo:

	docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 rabbitmq:3-management

Caso o container já tenha sido criado, execute o seguinte comando:

	docker container start some-rabbit.

Defina a solução para executar vários projetos de inicialização, caso ainda não esteja configurado.
	
	No gerenciado de soluções, clique com o botão direito e depois em "Definir projetos de inicialização".
	Na tela que abrir, selecione "Varios projetos de inicialização", em seguida, na coluna "ação", coloque os dois projetos para "Iniciar", clique em "Aplicar" depois em "OK".

## Executando a aplicação ##

Clique em iniciar no visual studio, será aberto duas telas, uma correspondende a aplicação responsável por enviar os dados para lista de leads, e outra responsável por consumir a lista de leads.

Você pode alimentar a lista de leads e assincronamente irá ver a aplicação consumidora exibir os dados do lead em tela, com o id gerado para o lead e data e hora de inserção desse registro.

Caso queira comprovar a assincronia não bloqueante, execute apenas o servido de envio (RabbitMQSender), registre quandos leads quiser, depois abra a aplicação consumidora (RabbitMQConsumer), você verá a aplicação iniciando e consumindo na fila e exibindo em tela os dados que estavam aguardando para serem consumidos.

Esta solução e exemplificativa, após o consumo da fila pelo consumer, atualmente o dado é salvo em um 
banco de dados em memória e permanece na fila por toda vida útil do container, em aplicações reais, 
após o dado ser consumido, normalmente o mesmo é retirado da fila depois de processado e, se for o caso, 
persistido em banco de dados pela aplicação consumidora.