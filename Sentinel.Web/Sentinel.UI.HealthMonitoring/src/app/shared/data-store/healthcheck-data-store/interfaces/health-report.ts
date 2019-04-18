export interface HealthReport {
    status: string;
    duration: string;
    results: HealthReportEntry[];
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
