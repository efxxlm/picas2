import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';

@Component({
  selector: 'app-registrar-actualiz-polizas-garantias',
  templateUrl: './registrar-actualiz-polizas-garantias.component.html',
  styleUrls: ['./registrar-actualiz-polizas-garantias.component.scss']
})
export class RegistrarActualizPolizasGarantiasComponent implements OnInit {
  verAyuda = false;
  addressForm = this.fb.group({
    numeroContrato: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(10) ] ) ],
  });
  myFilter = new FormControl();
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'numContrato',
    'tipoContrato',
    'contratista',
    'gestion'
  ];
  dataTable: any[] = [
    {
      numContrato:'N801800',
      tipoContrato:'Interventoria',
      contratista:'Constructora Colpatria',
      gestion: 1
    },
    {
      numContrato:'N801801',
      tipoContrato:'Obra',
      contratista:'Interventores S.A',
      gestion: 2
    },
    {
      numContrato:'N801999',
      tipoContrato:'Obra',
      contratista:'Constructora Colpatria',
      gestion: 3
    },
  ];
  contratosList: any[] = [
    {
      codigo: '1',
      numContrato: 'N801801'
    },
  ];
  filteredContrato: Observable<string[]>;
  selectedContract: any = '';
  estaEditando = false;
  constructor(
    private fb: FormBuilder, 
    private routes: Router
  ) { 
    //funcion Autocomplete
    this.filteredContrato = this.myFilter.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  ngOnInit(): void {
  }
  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();    
    if(value!="")
    {      
      let filtroportipo:string[]=[];
      this.contratosList.forEach(element => {        
        if(!filtroportipo.includes(element.numContrato))
        {
          filtroportipo.push(element.numContrato);
        }
      });
      let ret= filtroportipo.filter(x=> x.toLowerCase().indexOf(filterValue) === 0);      
      return ret;
    }
    else
    {
      return [];
    }
    
  }
  seleccionAutocomplete(nombre: string){
    this.selectedContract = nombre;
    let lista: any[] = [];
    this.contratosList.forEach(element => {
      if (element.numeroContrato) {
        lista.push(element);
      }
    });
    let ret = lista.filter(x => x.numeroContrato.toLowerCase() === nombre.toLowerCase());
    console.log(ret);
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

  validarBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/validarBalance', id]);
  }
  verDetalleEditarBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarBalance', id]);
  }
  verDetalleBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/verDetalleBalance', id]);
  }

}
