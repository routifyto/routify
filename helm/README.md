# Routify Helm Chart

## Introduction

This Helm chart deploys Routify, an open-source AI gateway that routes LLM requests to various providers with no code changes, on a Kubernetes cluster.

## Prerequisites

- Kubernetes 1.19+
- Helm 3.2.0+
- PV provisioner support in the underlying infrastructure (if persistence is enabled for PostgreSQL or Redis)

## Chart Details

This chart will deploy the following components of Routify:

1. API
2. Gateway
3. Frontend
4. Migrations (as a Kubernetes Job)
5. PostgreSQL (optional, can be disabled to use external database)
6. Redis (optional, can be disabled to use external Redis)

## Installing the Chart

To install the chart with the release name `my-routify`:

```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
helm dependency update ./helm/charts/routify
helm install my-routify ./helm/charts/routify
```

These commands add the Bitnami repo (for PostgreSQL and Redis dependencies), update the dependencies, and then install Routify on your Kubernetes cluster with default configurations.

## Uninstalling the Chart

To uninstall/delete the `my-routify` deployment:

```bash
helm uninstall my-routify
```

This command removes all the Kubernetes components associated with the chart and deletes the release.

## Configuration

The following table lists the configurable parameters of the Routify chart and their default values.

| Parameter                   | Description                                     | Default                     |
|-----------------------------|-------------------------------------------------|-----------------------------|
| `global.environment`        | Global environment setting                      | `Production`                |
| `postgresql.enabled`        | Deploy PostgreSQL                               | `true`                      |
| `postgresql.auth.username`  | PostgreSQL username                             | `routify_user`              |
| `postgresql.auth.password`  | PostgreSQL password                             | `routify_password`          |
| `postgresql.auth.database`  | PostgreSQL database name                        | `routify_ai`                |
| `redis.enabled`             | Deploy Redis                                    | `true`                      |
| `api.replicaCount`          | Number of API replicas                          | `1`                         |
| `api.image.repository`      | API image repository                            | `ghcr.io/routifyto/routify/api` |
| `api.image.tag`             | API image tag                                   | `latest`                    |
| `gateway.replicaCount`      | Number of Gateway replicas                      | `1`                         |
| `gateway.image.repository`  | Gateway image repository                        | `ghcr.io/routifyto/routify/gateway` |
| `gateway.image.tag`         | Gateway image tag                               | `latest`                    |
| `frontend.replicaCount`     | Number of Frontend replicas                     | `1`                         |
| `frontend.image.repository` | Frontend image repository                       | `ghcr.io/routifyto/routify/frontend` |
| `frontend.image.tag`        | Frontend image tag                              | `latest`                    |
| `migrations.image.repository` | Migrations image repository                   | `ghcr.io/routifyto/routify/migrations` |
| `migrations.image.tag`      | Migrations image tag                            | `latest`                    |

Specify each parameter using the `--set key=value[,key=value]` argument to `helm install`. For example:

```bash
helm install my-routify ./helm/charts/routify \
  --set postgresql.enabled=false \
  --set externalDatabase.host=my-external-postgresql
```

Alternatively, a YAML file that specifies the values for the parameters can be provided while installing the chart. For example:

```bash
helm install my-routify ./helm/charts/routify -f my-values.yaml
```

## External Database / Redis Support

To use an external database or Redis instance, set `postgresql.enabled` or `redis.enabled` to `false` and configure the `externalDatabase` or `externalRedis` parameters in your `values.yaml` file.

Example:

```yaml
postgresql:
  enabled: false

externalDatabase:
  host: my-external-postgresql
  port: 5432
  user: my-user
  password: my-password
  database: my-database

redis:
  enabled: false

externalRedis:
  host: my-external-redis
  port: 6379
```

## Migrations

The migrations are run as a Kubernetes Job. The job is set to run before the API service starts, ensuring that the database schema is up to date.

## Ingress

This chart includes an optional Ingress resource. To enable it, set `ingress.enabled` to `true` in your `values.yaml` file and configure the `ingress.hosts` section.

## Persistence

Both PostgreSQL and Redis can be configured to use persistent volumes. Refer to their respective charts' documentation for more details on configuring persistence.

## Upgrading

### To 0.1.0

This is the first release of the Routify Helm chart. There are no special upgrade considerations at this time.