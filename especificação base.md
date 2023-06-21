# Playercêntrico:
	Movement.cs
		InputManager (default): movement, jump.
		RigidBody
		ChildObject DetectorDeChao
	Combat.cs
		int vida;
		Spawn de prefab (bullet)
	Score.cs
		int score;
		grabCoin()
		gainLife()

	Bullet Object (prefab):
		OnCollisionEnter(collider2D)

	Interface: HitableObj
		(Td q da dano usa isso,ex: no código do inimigo: player.takeDmg(2))
		TakeDmg(int dmg) 

# Inimigocêntrico:
	behaviour.cs
		Player longe:
			Idle
		else:
			Dá pra bater nele:
				Bate nele
			else:
				Vai até ele
		Ao tomar dano:
			knockback

# Environmentcêntrico:
	Estalactite:
		detecta o player e cai; dps vira bloco normal; não atacável
	Bolinha:
		detecta o player e começa a se mover; ao receber tiros sua velocidade se altera;
		ao bater numa parede para
	Buraco:
		detectorDeChao guarda a posiçãod do último pulo feito no chão;
		ao cair, o player perde vida e é teleportado para tal posição.
	Lava:
		Similar ao buraco, mas sua hitbox se expande verticalmente periodicamente.

# Menucêntrico
	Vida e score persistem pelos níveis;
	O nível atual é salvo em um txt;
	Menu:
		Start
		Continue
		Config -> audio
	Pausa: :)





