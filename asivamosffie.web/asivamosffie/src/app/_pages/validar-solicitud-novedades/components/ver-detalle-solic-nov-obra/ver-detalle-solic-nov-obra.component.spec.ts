import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleSolicNovObraComponent } from './ver-detalle-solic-nov-obra.component';

describe('VerDetalleSolicNovObraComponent', () => {
  let component: VerDetalleSolicNovObraComponent;
  let fixture: ComponentFixture<VerDetalleSolicNovObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleSolicNovObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleSolicNovObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
