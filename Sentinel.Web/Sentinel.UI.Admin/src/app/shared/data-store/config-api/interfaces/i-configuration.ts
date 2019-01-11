import { IAppsettingValidate } from './i-appsetting-validate';
export interface IConfiguration {
    activeConfigs?: IConfigItem[];
    activeConfigObject?: any;
    appKeyVaultSettingsClientIdFound?: boolean;
    appKeyVaultSettingsClientIdMessage?: string;
    appKeyVaultSettingsUrlFound?: boolean;
    appKeyVaultSettingsUrlMessage?: string;
    certFound?: boolean;
    certMessage?: string;
    errorMessage?: string;
    keyvaultAdded?: boolean;
    keyvaultMessage?: string;
    startupSecret?: string;
    thumbprintFound?: boolean;
    thumbprintMessage?: string;
}

export interface IConfigItem {
    key: string;
    value: string;
    appsettingValidate?: IAppsettingValidate;
    httpCallResult?: any;
}
