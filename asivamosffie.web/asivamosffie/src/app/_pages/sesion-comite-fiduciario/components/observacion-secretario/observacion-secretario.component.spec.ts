import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionSecretarioComponent } from './observacion-secretario.component';

describe('ObservacionSecretarioComponent', () => {
  let component: ObservacionSecretarioComponent;
  let fixture: ComponentFixture<ObservacionSecretarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionSecretarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionSecretarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
