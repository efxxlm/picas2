import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

@Component({
  selector: 'app-registrar-solicitud-interventoria',
  templateUrl: './registrar-solicitud-interventoria.component.html',
  styleUrls: ['./registrar-solicitud-interventoria.component.scss']
})
export class RegistrarSolicitudInterventoriaComponent implements OnInit {

  numeroContrato = new FormControl();
  novedadAplicada = new FormControl();
  filteredOptions: Observable<string[]>;
  options: string[] = ['One', 'Two', 'Three'];

  novedadesArray = [
    { name: 'contrato 1', value: '1' },
    { name: 'contrato 2', value: '2' },
    { name: 'Proyecto', value: 'Proyecto' }
  ];

  constructor() { }

  ngOnInit() {
    this.filteredOptions = this.numeroContrato.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().indexOf(filterValue) === 0);
  }

}
