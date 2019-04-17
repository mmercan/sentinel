export interface HealthReport {
    status: string;
    duration: string;
    results: HealthReportEntry[];
}

export interface HealthReportEntry {
    status: string;
    description: string;
    duration: string;
    data: any;
    exception: string;
}
