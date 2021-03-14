import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Form, FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { DefensaJudicial, DefensaJudicialContratacionProyecto, DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-contratos-asociados-dj',
  templateUrl: './form-contratos-asociados-dj.component.html',
  styleUrls: ['./form-contratos-asociados-dj.component.scss']
})
export class FormContratosAsociadosDjComponent implements OnInit {
  displayedColumns: string[] = ['nombreContratista', 'institucionEducativa', 'codigoDane', 'sede', 'sedeCodigo', 'contratacionProyectoId'];
  dataSource = new MatTableDataSource([]);
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @Input() legitimacion: boolean;
  @Input() tipoProceso: string;
  @Input() defensaJudicial: DefensaJudicial;
  @Input() esrojo: boolean;
  dataTable: any[] = [
  ];
  myControl = new FormArray([]);
  filteredName: Observable<string[]>;
  formContratista: FormGroup;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  contratosArray = [];
  contratos = [];
  listProyectos: any[] = [];
  listProyectosSeleccion: any[] = [];
  listContrattoscompletos = [];
  estaEditando = false;
  constructor(private fb: FormBuilder, private defensaService: DefensaJudicialService,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router) {
    this.crearFormulario();
  }

  cargarRegistro() {
    //this.ngOnInit().then(() => {
    console.log("form");
    console.log(this.defensaJudicial);
    console.log(this.legitimacion);
    console.log(this.tipoProceso);
    if (Object.keys(this.defensaJudicial).length > 0) {
      this.estaEditando = true;
      this.formContratista.markAllAsTouched();
      this.formContratista.get('numeroContratos').setValue(this.defensaJudicial.cantContratos);

      let listaContratos: any[] = [];

      this.defensaJudicial.defensaJudicialContratacionProyecto.forEach(element => {
        if (listaContratos.filter(r => r.numeroContrato == element.numeroContrato).length === 0)
          listaContratos.push(this.contratos.filter(x => x.numeroContrato == element.numeroContrato)[0]);
      });

      let i = 0;
      console.log(listaContratos);
      listaContratos.forEach(c => {
        //this.myControl.controls[i].setValue(c.numeroContrato);
        this.perfiles.value.contrato = c.contratoId;
        this.defensaService.GetListProyectsByContract(c.contratoId).subscribe(response => {
          this.listProyectosSeleccion = response;
          this.dataTable = response;
          let alguno = false;
          this.dataTable.forEach(element2 => {
            this.defensaJudicial.defensaJudicialContratacionProyecto.forEach(test => {
              if (element2.proyectoId == test.contratacionProyecto.proyectoId) {
                element2.checked = true;
                alguno = true;
              }
            });
          });
          this.dataSource[i] = new MatTableDataSource(this.dataTable);
          this.dataSource[i].paginator = this.paginator;
          this.dataSource[i].sort = this.sort;
          this.listContrattoscompletos[i] = alguno;
          this.myControl.controls[i].setValue(c.numeroContrato);
          i++;
        });

      });


      /*
      let i=0;
      this.defensaJudicial.defensaJudicialContratacionProyecto.forEach(element => {
        this.myControl.controls[i].setValue(element.numeroContrato);
        let contrato=this.contratos.filter(x=>x.numeroContrato==element.numeroContrato);
        this.perfiles.value.contrato = contrato[0].contratoId;
      this.defensaService.GetListProyectsByContract(contrato[0].contratoId).subscribe(response=>{
        console.log(contrato[0].contratoId);
        this.listProyectosSeleccion=response;
        this.dataTable=response;
        let alguno=false;
        this.dataTable.forEach(element2 => {
          this.defensaJudicial.defensaJudicialContratacionProyecto.forEach(test => {
            if(element2.proyectoId==test.contratacionProyecto.proyectoId)
            {
              element2.checked=true;
              alguno=true;
            }
          });
        });
        this.dataSource[i] = new MatTableDataSource(this.dataTable);
        console.log("Datatable: ",this.dataTable);
        console.log("Datasource: ");
        this.dataSource[i].paginator = this.paginator;
        this.dataSource[i].sort = this.sort;
        this.listContrattoscompletos[i]=alguno;
        i++;
      });
      });
      */
    }
    //});
  }

  ngOnInit(): void {
    this.defensaService.GetListContract().subscribe(response => {
      //this.contratosArray=response.map(x=>x.numeroContrato);
      this.contratos = response;
    });
    this.formContratista.get('numeroContratos').valueChanges
      .subscribe(value => {
        this.perfiles.clear();
        for (let i = 0; i < Number(value); i++) {
          this.perfiles.push(
            this.fb.group(
              {
                contrato: [null]
              }
            )
          )
          let control = new FormControl();

          this.filteredName = control.valueChanges.pipe(
            startWith(''),
            map(values => this._filter(values))
          );
          this.myControl.push(control);
        }
      });


  };

  get perfiles() {
    return this.formContratista.get('perfiles') as FormArray;
  };

  get numeroRadicado() {
    let numero;
    Object.values(this.formContratista.controls).forEach(control => {
      if (control instanceof FormArray) {
        Object.values(control.controls).forEach(control => {
          numero = control.get('numeroRadicadoFfieAprobacionCv') as FormArray;
        })
      }
    })
    return numero;
  };

  private _filter(value: string): string[] {
    console.log("intentnado filtrar" + value);
    const filterValue = value.toLowerCase();
    if (value != "") {
      let filtroportipo: string[] = [];
      this.contratos.forEach(element => {
        if (element.numeroContrato == value) {
          if (!filtroportipo.includes(element.numeroContrato)) {
            filtroportipo.push(element.numeroContrato);
          }
        }
      });
      let ret = filtroportipo.filter(x => x.toLowerCase().indexOf(filterValue) === 0);
      return ret;
    }
    else {
      return [];
    }

  }
  getContratos(trigger: MatAutocompleteTrigger, i:number) {
    if (this.formContratista.get('perfiles').value[i].contrato !== null) {
        this.defensaService.GetListContractAutoComplete(this.formContratista.get('perfiles').value[i].contrato)
          .subscribe(response => {
            this.contratosArray[i] = response;
            if ( response.length > 0 ) {
              trigger.openPanel();
            }
          });
    }
  }
  seleccionAutocomplete(id: any, i: number) {
    this.perfiles.value.contrato = id.contratoId;
    this.defensaService.GetListProyectsByContract(this.perfiles.value.contrato).subscribe(response => {
      this.listProyectosSeleccion = response;
      this.dataTable = response;
      this.dataSource[i] = new MatTableDataSource(this.dataTable);
      this.dataSource[i].paginator = this.paginator;
      this.dataSource[i].sort = this.sort;
      this.listContrattoscompletos[i] = false;
      console.log(this.dataSource);
    });
  }
  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario() {
    this.formContratista = this.fb.group({
      numeroContratos: [''],
      perfiles: this.fb.array([])
    });
  };
  openDialogSiNo(modalTitle: string, modalText: string,id) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.eliminarPerfil(id); 
      }           
    });
  }
  eliminar(id)
  {
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",id);
  }
  eliminarPerfil(numeroPerfil: number) {
    this.perfiles.removeAt(numeroPerfil);
    this.formContratista.patchValue({
      numeroContratos: `${this.perfiles.length}`
    });
  };
  number(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8) { return true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { return true; } // 0
    if (tecla === 49) { return true; } // 1
    if (tecla === 50) { return true; } // 2
    if (tecla === 51) { return true; } // 3
    if (tecla === 52) { return true; } // 4
    if (tecla === 53) { return true; } // 5
    if (tecla === 54) { return true; } // 6
    if (tecla === 55) { return true; } // 7
    if (tecla === 56) { return true; } // 8
    if (tecla === 57) { return true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    return patron.test(te);
  }

  agregarNumeroRadicado() {
    this.numeroRadicado.push(this.fb.control(''))
  }

  eliminarNumeroRadicado(numeroRadicado: number) {
    this.numeroRadicado.removeAt(numeroRadicado);
  };

  guardar() {
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    console.log(this.formContratista);
    console.log(this.listProyectos);
    let defContraProyecto: DefensaJudicialContratacionProyecto[] = [];
    this.listProyectos.forEach(element => {
      defContraProyecto.push({
        defensaJudicialContratacionProyectoId: 0,
        contratacionProyectoId: element.contratacionProyectoId,
        esCompleto: true
      });
    });
    let defensaJudicial = this.defensaJudicial;
    if (!this.defensaJudicial.defensaJudicialId || this.defensaJudicial.defensaJudicialId == 0) {
      defensaJudicial = {
        defensaJudicialId: this.defensaJudicial.defensaJudicialId,
        tipoProcesoCodigo: this.tipoProceso,
        esLegitimacionActiva: this.legitimacion,
      };
    }else{
      this.tipoProceso != null ? defensaJudicial.tipoProcesoCodigo = this.tipoProceso : this.defensaJudicial.tipoProcesoCodigo;
      this.legitimacion != null ? defensaJudicial.esLegitimacionActiva = this.legitimacion : this.defensaJudicial.esLegitimacionActiva;
    }
    defensaJudicial.cantContratos = this.formContratista.get('numeroContratos').value;
    defensaJudicial.defensaJudicialContratacionProyecto = defContraProyecto;
    if(this.tipoProceso==null || this.legitimacion==null){
      this.openDialog('', '<b>Falta registrar información.</b>');
    }
    else{
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial)
      .subscribe((response: Respuesta) => {
        this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
      },
      err => {
        this.openDialog('', err.message);
      });
    }

  }

  openDialog(modalTitle: string, modalText: string, redirect?: boolean, id?: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (redirect) {
      dialogRef.afterClosed().subscribe(result => {
        if (id > 0 && this.defensaJudicial.defensaJudicialId != id) {
          this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/" + id], {});
        }
        else {
          if(this.defensaJudicial.defensaJudicialId == id){
            location.reload();
          }else{
            this.router.navigate(["/gestionarProcesoDefensaJudicial"], {});
          }
        }
      });
    }
  }

  addProject(idproyecto: any) {

    console.log(idproyecto);
    if (this.listProyectos.includes(idproyecto)) {
      var index = this.listProyectos.indexOf(idproyecto);
      if (index !== -1) {
        this.listProyectos.splice(index, 1);
      }
    }
    else {
      this.listProyectos.push({ contratacionProyectoId: idproyecto });
    }

    console.log(this.listProyectos);
  }
}
