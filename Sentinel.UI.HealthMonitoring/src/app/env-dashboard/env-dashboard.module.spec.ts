import { EnvDashboardModule } from './env-dashboard.module';

describe('EnvDashboardModule', () => {
  let envDashboardModule: EnvDashboardModule;

  beforeEach(() => {
    envDashboardModule = new EnvDashboardModule();
  });

  it('should create an instance', () => {
    expect(envDashboardModule).toBeTruthy();
  });
});
