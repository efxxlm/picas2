import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-razon-tipo-actualizacion-rapg',
  templateUrl: './razon-tipo-actualizacion-rapg.component.html',
  styleUrls: ['./razon-tipo-actualizacion-rapg.component.scss']
})
export class RazonTipoActualizacionRapgComponent implements OnInit {
  addressForm = this.fb.group({
    razonActualizacion: [null, Validators.required],
    fechaExpedicion: [null, Validators.required],
    polizasYSeguros: [null, Validators.required],
    tipoActualizacion: [null, Validators.required]
  });
  razonActualizacionArray : any[] = [
    {
      valor:'1',
      nombre: 'Terminación de contrato'
    }
  ]
  polizasYSegurosArray: Dominio[] = [];
  tipoActualizacionArray: any[] = [{
    codigo: '1',
    nombre: 'Fecha'
  },
  {
    codigo: '2',
    nombre: 'Valor'
  }];
  obj1: boolean;
  obj2: boolean;
  obj3: boolean;
  obj4: boolean;
  estaEditando = false;
  constructor(private common: CommonService, private fb: FormBuilder, private router: Router) {
    this.common.listaGarantiasPolizas().subscribe(data0 => {
      this.polizasYSegurosArray = data0;
    });
  }

  ngOnInit(): void {
  }
  
  getvalues(values: Dominio[]) {
    const buenManejo = values.find(value => value.codigo == "1");
    const garantiaObra = values.find(value => value.codigo == "2");
    const pCumplimiento = values.find(value => value.codigo == "3");
    const polizasYSeguros = values.find(value => value.codigo == "4");

    buenManejo ? this.obj1 = true : this.obj1 = false;
    garantiaObra ? this.obj2 = true : this.obj2 = false;
    pCumplimiento ? this.obj3 = true : this.obj3 = false;
    polizasYSeguros ? this.obj4 = true : this.obj4 = false;

  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  eliminarobjeto(obj){
    obj = false;
  }
  onSubmit() {
    this.estaEditando = true;
  }

}
