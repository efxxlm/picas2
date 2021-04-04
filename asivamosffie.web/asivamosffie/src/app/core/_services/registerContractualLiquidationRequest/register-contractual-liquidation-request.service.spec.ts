import { TestBed } from '@angular/core/testing';
import { RegisterContractualLiquidationRequestService } from './register-contractual-liquidation-request.service';


describe('RegisterContractualLiquidationRequestService', () => {
  let service: RegisterContractualLiquidationRequestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegisterContractualLiquidationRequestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
