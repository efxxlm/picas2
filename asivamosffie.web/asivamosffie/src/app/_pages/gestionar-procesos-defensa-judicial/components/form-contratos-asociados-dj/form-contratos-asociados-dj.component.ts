import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
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
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;

  dataTable: any[] = [
  ];
  myControl = new FormControl();
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
  contratos=[];
  listProyectos: any[]=[];
  listProyectosSeleccion: any[]=[];

  constructor ( private fb: FormBuilder, private defensaService:DefensaJudicialService,
    public dialog: MatDialog,    
    private route: ActivatedRoute,
    private router: Router ) {
    this.crearFormulario();    
  }
  
  cargarRegistro() {
    //this.ngOnInit().then(() => {
      console.log("form");
      console.log(this.defensaJudicial);
      console.log(this.legitimacion);
      console.log(this.tipoProceso);
      if(Object.keys(this.defensaJudicial).length>0)
      {
        this.formContratista.get( 'numeroContratos' ).setValue(this.defensaJudicial.cantContratos);
      }  
    //});
  }

  ngOnInit(): void {
    
    this.defensaService.GetListContract().subscribe(response=>{
      this.contratosArray=response.map(x=>x.numeroContrato);
      this.contratos=response;
    });
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                contrato: [ null ]                
              }
            ) 
          )
        }
      } );

      this.filteredName = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value))
      );
  };

	  get perfiles () {
		return this.formContratista.get( 'perfiles' ) as FormArray;
	  };

  get numeroRadicado () {
    let numero;
    Object.values( this.formContratista.controls ).forEach( control => {
      if ( control instanceof FormArray ) {
        Object.values( control.controls ).forEach( control => {
          numero = control.get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
        } )
      }
    } )
    return numero;
  };

  private _filter(value: string): string[] {
    console.log("intentnado filtrar"+value);
    const filterValue = value.toLowerCase();    
    if(value!="")
    {      
      let filtroportipo:string[]=[];
      this.contratos.forEach(element => {        
        if(element.numeroContrato==value)
        {
          if(!filtroportipo.includes(element.numeroContrato))
          {
            filtroportipo.push(element.numeroContrato);
          }
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

  seleccionAutocomplete(id:any){
    this.perfiles.value.contrato = id;
    let contrato=this.contratos.filter(x=>x.numeroContrato==id);
    this.defensaService.GetListProyectsByContract(contrato[0].contratoId).subscribe(response=>{
      this.listProyectosSeleccion=response;
      this.dataTable=response;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
    
    
    
  }
  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroContratos: `${ this.perfiles.length }`
    });
  };

  agregarNumeroRadicado () {
    this.numeroRadicado.push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado.removeAt( numeroRadicado );
  };

  guardar () {
    console.log( this.formContratista );
    console.log(this.listProyectos);
    let defContraProyecto:DefensaJudicialContratacionProyecto[]=[];
    this.listProyectos.forEach(element => {
      defContraProyecto.push({
        defensaJudicialContratacionProyectoId:0,
        contratacionProyectoId:element.contratacionProyectoId,
        esCompleto:true
      });
    });
    
    let defensaJudicial:DefensaJudicial={
      defensaJudicialId:0,
      legitimacionCodigo:'',
      tipoProcesoCodigo:'',
      numeroProceso:'',
      cantContratos:this.formContratista.get( 'numeroContratos' ).value,
      estadoProcesoCodigo:'',
      solicitudId:0,
      esLegitimacionActiva:null,
      esCompleto:false,
      defensaJudicialContratacionProyecto:defContraProyecto
    };
      console.log(defensaJudicial);
      this.defensaService.CreateOrEditDefensaJudicial(defensaJudicial).subscribe(
        response=>{
          this.openDialog('', `<b>${response.message}</b>`,true,response.data?response.data.defensaJudicialId:0);
        }
      );

  }

  openDialog(modalTitle: string, modalText: string,redirect?:boolean,id?:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(redirect)
    {
      dialogRef.afterClosed().subscribe(result => {
          if(id>0)
          {
            this.router.navigate(["/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial/"+id], {});
          }                  
      });
    }
  }

  addProject(idproyecto:any)
  {
    
    console.log(idproyecto);
      if(this.listProyectos.includes(idproyecto))
      {
        var index = this.listProyectos.indexOf(idproyecto);
        if (index !== -1) {
          this.listProyectos.splice(index, 1);
        }      
      }
      else
      {
        this.listProyectos.push({contratacionProyectoId:idproyecto});
      }
        
    console.log(this.listProyectos);
  }
}
