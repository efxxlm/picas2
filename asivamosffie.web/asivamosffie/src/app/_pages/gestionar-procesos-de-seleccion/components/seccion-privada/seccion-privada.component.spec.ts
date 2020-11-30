import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeccionPrivadaComponent } from './seccion-privada.component';

describe('SeccionPrivadaComponent', () => {
  let component: SeccionPrivadaComponent;
  let fixture: ComponentFixture<SeccionPrivadaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeccionPrivadaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeccionPrivadaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
