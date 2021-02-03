import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FuentesUsosGbftrecComponent } from './fuentes-usos-gbftrec.component';

describe('FuentesUsosGbftrecComponent', () => {
  let component: FuentesUsosGbftrecComponent;
  let fixture: ComponentFixture<FuentesUsosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FuentesUsosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FuentesUsosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
