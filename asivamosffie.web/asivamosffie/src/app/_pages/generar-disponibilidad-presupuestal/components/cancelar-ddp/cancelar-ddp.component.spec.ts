import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CancelarDdpComponent } from './cancelar-ddp.component';

describe('CancelarDdpComponent', () => {
  let component: CancelarDdpComponent;
  let fixture: ComponentFixture<CancelarDdpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CancelarDdpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CancelarDdpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
