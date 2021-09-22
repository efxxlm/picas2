import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaPorcntjParticGogComponent } from './tabla-porcntj-partic-gog.component';

describe('TablaPorcntjParticGogComponent', () => {
  let component: TablaPorcntjParticGogComponent;
  let fixture: ComponentFixture<TablaPorcntjParticGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaPorcntjParticGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaPorcntjParticGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
