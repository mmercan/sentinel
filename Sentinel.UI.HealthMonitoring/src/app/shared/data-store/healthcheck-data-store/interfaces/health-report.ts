export interface HealthReport {
    status: string;
    duration: string;
    results: HealthReportEntry[];
    url?: string;
    servicename?: string;
}

export interface HealthReportEntry {
    name: string;
    status: string;
    type: string;
    description: string;
    duration: string;
    data: any;
    exception: string;
}

export interface HealthReportUrl {
    isaliveandwell: string;
    name: string;
}