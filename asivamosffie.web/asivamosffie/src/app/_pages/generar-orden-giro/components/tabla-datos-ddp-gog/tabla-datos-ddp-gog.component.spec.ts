import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDatosDdpGogComponent } from './tabla-datos-ddp-gog.component';

describe('TablaDatosDdpGogComponent', () => {
  let component: TablaDatosDdpGogComponent;
  let fixture: ComponentFixture<TablaDatosDdpGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDatosDdpGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDatosDdpGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
