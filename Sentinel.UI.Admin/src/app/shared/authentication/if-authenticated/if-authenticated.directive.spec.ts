import { IfAuthenticatedDirective } from './if-authenticated.directive';

describe('IfAuthenticatedDirective', () => {
  it('should create an instance', () => {
    const directive = new IfAuthenticatedDirective();
    expect(directive).toBeTruthy();
  });
});
