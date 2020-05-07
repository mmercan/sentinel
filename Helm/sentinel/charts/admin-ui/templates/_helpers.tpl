{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "Sentinel.UI.Admin.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "Sentinel.UI.Admin.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- $name := default .Chart.Name .Values.nameOverride -}}
{{- if contains $name .Release.Name -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end -}}
{{- end -}}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "Sentinel.UI.Admin.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{- define "Sentinel.UI.Admin.labels" -}}
helm.sh/chart: {{ include "Sentinel.UI.Admin.chart" . }}
{{ include "Sentinel.UI.Admin.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end -}}

{{/*
Selector labels
*/}}
{{- define "Sentinel.UI.Admin.selectorLabels" -}}
app: {{ include "Sentinel.UI.Admin.name" . }}
version: {{ .Chart.AppVersion  | quote }}
app.kubernetes.io/name: {{ include "Sentinel.UI.Admin.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
branch:  {{ .Values.branch }}
{{- end -}}

{{- define "Sentinel.UI.Admin.annotations" -}}
azure-pipelines/run: {{ .Values.azurepipelines.run }}
azure-pipelines/pipeline: {{ .Values.azurepipelines.pipeline }}
azure-pipelines/pipelineId: {{ .Values.azurepipelines.pipelineId }}
azure-pipelines/jobName: {{ .Values.azurepipelines.jobName }}
azure-pipelines/runuri: {{ .Values.azurepipelines.runuri }}
azure-pipelines/project: {{ .Values.azurepipelines.project }}
azure-pipelines/org: {{ .Values.azurepipelines.org }}
{{- end -}}