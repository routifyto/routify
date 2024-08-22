{{/*
Expand the name of the chart.
*/}}
{{- define "routify.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "routify.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "routify.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "routify.labels" -}}
helm.sh/chart: {{ include "routify.chart" . }}
{{ include "routify.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "routify.selectorLabels" -}}
app.kubernetes.io/name: {{ include "routify.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "routify.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "routify.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Get the Postgres host
*/}}
{{- define "routify.postgresHost" -}}
{{- if .Values.postgresql.enabled }}
{{- printf "%s-%s" .Release.Name "postgresql" | trunc 63 | trimSuffix "-" -}}
{{- else }}
{{- .Values.externalDatabase.host -}}
{{- end -}}
{{- end -}}

{{/*
Get the Postgres port
*/}}
{{- define "routify.postgresPort" -}}
{{- if .Values.postgresql.enabled }}
{{- .Values.postgresql.service.port -}}
{{- else }}
{{- .Values.externalDatabase.port -}}
{{- end -}}
{{- end -}}

{{/*
Get the Postgres user
*/}}
{{- define "routify.postgresUser" -}}
{{- if .Values.postgresql.enabled }}
{{- .Values.postgresql.auth.username -}}
{{- else }}
{{- .Values.externalDatabase.user -}}
{{- end -}}
{{- end -}}

{{/*
Get the Postgres password
*/}}
{{- define "routify.postgresPassword" -}}
{{- if .Values.postgresql.enabled }}
{{- .Values.postgresql.auth.password -}}
{{- else }}
{{- .Values.externalDatabase.password -}}
{{- end -}}
{{- end -}}

{{/*
Get the Postgres database name
*/}}
{{- define "routify.postgresDatabase" -}}
{{- if .Values.postgresql.enabled }}
{{- .Values.postgresql.auth.database -}}
{{- else }}
{{- .Values.externalDatabase.database -}}
{{- end -}}
{{- end -}}

{{/*
Get the Redis host
*/}}
{{- define "routify.redisHost" -}}
{{- if .Values.redis.enabled }}
{{- printf "%s-redis-master" .Release.Name -}}
{{- else }}
{{- .Values.externalRedis.host -}}
{{- end -}}
{{- end -}}

{{/*
Get the Redis port
*/}}
{{- define "routify.redisPort" -}}
{{- if .Values.redis.enabled }}
6379
{{- else }}
{{- .Values.externalRedis.port -}}
{{- end -}}
{{- end -}}

{{/*
Get the Redis password or empty string if auth is disabled
*/}}
{{- define "routify.redisPassword" -}}
{{- if and .Values.redis.enabled .Values.redis.auth.enabled }}
{{- .Values.redis.auth.password -}}
{{- else if and .Values.redis.enabled (not .Values.redis.auth.enabled) }}
{{- "" -}}
{{- else }}
{{- .Values.externalRedis.password -}}
{{- end -}}
{{- end -}}

{{/*
Get the Redis connection string
*/}}
{{- define "routify.redisConnectionString" -}}
{{- $password := include "routify.redisPassword" . -}}
{{- if ne $password "" }}
{{- printf "%s:%s,password=%s" (include "routify.redisHost" .) (include "routify.redisPort" .) $password -}}
{{- else }}
{{- printf "%s:%s" (include "routify.redisHost" .) (include "routify.redisPort" .) -}}
{{- end -}}
{{- end -}}