import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormArray } from '@angular/forms';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-descuentos-gog',
  templateUrl: './form-descuentos-gog.component.html',
  styleUrls: ['./form-descuentos-gog.component.scss']
})
export class FormDescuentosGogComponent implements OnInit {
  listaCriterios: Dominio[] = [];
  listaConceptoPago: any[] = [
    {
      nombre: 'Demolición',
      codigo: '1'
    },
    {
      nombre: 'Cimentación',
      codigo: '2'
    }
  ];
  tipoAportantesArray: any[] = [
    {
      nombre: 'ET',
      codigo: '1'
    }
  ];
  nombreAportantesArray: any[] = [
    {
      nombre: 'Alcaldía de Susacón',
      codigo: '1'
    }
  ];
  fuenteRecursosArray: any[] = [
    {
      nombre: 'Contingencias',
      codigo: '1'
    }
  ];
  addressForm = this.fb.group({
    criterios: [null, Validators.required],
    conceptosdePago: [null, Validators.required],
    aportantes: this.fb.array([]),
    tipoAportante: [ null ],
    nombreAportante: [ '' ],
    fuenteRecursos: [ '' ],
    valorDescuento: [ '' ],
  });
  //Variables para temas de maquetación
  obj1: boolean;
  obj2: boolean;
  objD1: boolean;
  objD2: boolean;
  constructor(public common: CommonService, private fb: FormBuilder) {
    this.common.criteriosDePago().subscribe((data0: any) => {
      this.listaCriterios = data0;
    });
  }
  get aportantes () {
    return this.addressForm.get( 'aportantes' ) as FormArray;
  };
  ngOnInit(): void {
    /*
    this.addressForm.get( 'aportantes' ).valueChanges
    .subscribe( value => {
      this.aportantes.clear();
      for ( let i = 0; i < Number(value); i++ ) {
        this.aportantes.push( 
          this.fb.group(
            {
              tipoAportante: [ null ],
              nombreAportante: [ '' ],
              fuenteRecursos: [ '' ],
              valorDescuento: [ '' ],
            }
          ) 
        )
      }
    } );
    */
  }
  getvalues(values: Dominio[]) {
    const ej1 = values.find(value => value.codigo == "1");
    const ej2 = values.find(value => value.codigo == "2");
    ej1 ? this.obj1 = true : this.obj1 = false;
    ej2 ? this.obj2 = true : this.obj2 = false;
  }
  obtenerConceptos(values: any[]) {
    const ej1 = values.find(value => value.codigo == "1");
    const ej2 = values.find(value => value.codigo == "2");
    ej1 ? this.objD1 = true : this.objD1 = false;
    ej2 ? this.objD2 = true : this.objD2 = false;
  }
  onSubmit() {

  }
}
