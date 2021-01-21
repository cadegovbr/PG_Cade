[![Build Status](https://cgudev.visualstudio.com/PGD/_apis/build/status/PGD?branchName=master)](https://cgudev.visualstudio.com/PGD/_build/latest?definitionId=3&branchName=master)

Regras para o desenvolvimento do projeto no git

**Branches Fixas**

o repositório tem 2 branches fixas:
- master: que deve sempre refletir a versão em produção. Merge na master deve 
ser feito apenas quando for gerada nova versão em produção. Os builds para 
treinamento e produção rodam nessa branch.

- develop: deve refletir a versão que está sendo homologada pelos gestores. O buid
para ambiente de homologacao roda nesta branch. 


**Novas Features**

A cada nova entrega ou feature a ser desenvolvida no sistema, deverá ser aberta
uma nova branch, a partir da branch develop. Por exemplo, ao ser iniciada uma
branch de desenvolvimento da iteração 3, deve ser aberta a branch Iteracao3 a 
partir da develop.

Quando a entrega estiver pronta para ser homologada, deve ser realizado no gitlab
um merge request da branch da feature com a branch develop.

Nunca deverá ser commitada diretamente na develop uma entrega de feature.


**Bugs encontrados durante a homologacao**

Excepcionalmente para uma correção de bug será aceito commit diretamente na branch
develop, para que a correção seja refletida rapidamente no ambiente de homologacao.


**Bugs encontrados em produção**

Uma branch de hotfix deverá ser aberta a partir da master (exemplo hotfix_3_1_3)
e ao ser completada deve ser realizado um merge request para a master. Nunca deve
ser realizado commit direto na master.



referência: http://nvie.com/posts/a-successful-git-branching-model/
