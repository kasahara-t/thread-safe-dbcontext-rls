test:
	trap 'docker compose down' EXIT; \
	docker compose up --build --abort-on-container-exit