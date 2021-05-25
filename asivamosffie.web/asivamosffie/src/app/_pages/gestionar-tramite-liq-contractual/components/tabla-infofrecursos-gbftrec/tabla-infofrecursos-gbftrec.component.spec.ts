import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInfofrecursosGbftrecComponent } from './tabla-infofrecursos-gbftrec.component';

describe('TablaInfofrecursosGbftrecComponent', () => {
  let component: TablaInfofrecursosGbftrecComponent;
  let fixture: ComponentFixture<TablaInfofrecursosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInfofrecursosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInfofrecursosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
