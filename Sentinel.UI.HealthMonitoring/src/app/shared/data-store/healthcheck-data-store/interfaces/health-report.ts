export interface IHealthReport {
    status: string;
    duration: string;
    results: IHealthReportEntry[];
    url?: string;
    servicename?: string;
}

export interface IHealthReportEntry {
    name: string;
    status: string;
    type: string;
    description: string;
    duration: string;
    data: any;
    exception: string;
}

export interface IHealthReportUrl {
    isaliveandwell: string;
    name: string;
}
