import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleReclamacionAsegCcComponent } from './verdetalle-reclamacion-aseg-cc.component';

describe('VerdetalleReclamacionAsegCcComponent', () => {
  let component: VerdetalleReclamacionAsegCcComponent;
  let fixture: ComponentFixture<VerdetalleReclamacionAsegCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleReclamacionAsegCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleReclamacionAsegCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
