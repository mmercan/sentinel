export interface IEnvironment {
    environmentName?: string;
    applicationName?: string;
    webRootPath?: string;
    webRootFileProvider?: {
        root: string;
    };
    contentRootPath?: string;
    contentRootFileProvider?: {
        root: string;
    };
}
